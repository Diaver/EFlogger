using System;
using System.Data;
using System.Data.Common;
using EFlogger.Profiling.Data;
using EFlogger.Profiling.Helpers;

namespace EFlogger.Profiling
{
    public class MiniProfiler : IDbProfiler
    {
        private static MiniProfiler _profiler;

        public static MiniProfiler Current
        {
            get
            {
                if (_profiler == null)
                {
                    _profiler = new MiniProfiler();
                }

                return _profiler;
            }
        }

        private MiniProfiler()
        {
            IsActive = true;
            SqlProfiler = new SqlProfiler(this);
            _sw = StopwatchWrapper.StartNew();
            Id = Guid.NewGuid();
            Root = new Timing(this, null, "empty");
        }

        public Timing Root { get; set; }

        public decimal GetRoundedMilliseconds(long ticks)
        {
            long z = 10000 * ticks;
            decimal timesTen = (int)(z / _sw.Frequency);
            return timesTen / 10;
        }

        internal decimal GetDurationMilliseconds(long startTicks)
        {
            return GetRoundedMilliseconds(ElapsedTicks - startTicks);
        }

        public Guid Id { get; set; }

        private readonly IStopwatch _sw;

        internal long ElapsedTicks
        {
            get { return _sw.ElapsedTicks; }
        }

        internal IStopwatch Stopwatch
        {
            get { return _sw; }
        }


        public Timing Head { get; set; }

        public static CustomTiming CustomTiming(string category, string commandString, string executeType = null)
        {
            return CustomTimingIf(category, commandString, 0, executeType: executeType);
        }

        public static CustomTiming CustomTimingIf(string category, string commandString, decimal minSaveMs, string executeType = null)
        {


            var result = new CustomTiming(Current, commandString, minSaveMs)
            {
                ExecuteType = executeType,
                Category = category
            };

            // THREADING: revisit
            Current.Head.AddCustomTiming(category, result);

            return result;
        }

        /// <summary>
        /// Contains information about queries executed during this profiling session.
        /// </summary>
        private SqlProfiler SqlProfiler { get; set; }

        void IDbProfiler.ExecuteStart(ProfiledDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            SqlProfiler.ExecuteStart(profiledDbCommand, executeType);
        }

        void IDbProfiler.ExecuteFinish(ProfiledDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
            if (reader != null)
            {
                SqlProfiler.ExecuteFinish(profiledDbCommand, executeType, reader);
            }
            else
            {
                SqlProfiler.ExecuteFinish(profiledDbCommand, executeType);
            }
        }

        void IDbProfiler.ReaderFinish(IDataReader reader)
        {
            SqlProfiler.ReaderFinish(reader);
        }

        void IDbProfiler.OnError(ProfiledDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
            SqlProfiler.Exception(profiledDbCommand, executeType, exception);
        }


        bool _isActive;
        bool IDbProfiler.IsActive { get { return _isActive; } }
        internal bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
    }
}
