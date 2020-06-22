using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Linq;
using EFlogger.Profiling.Data;

namespace EFlogger.Profiling
{
    /// <summary>
    /// Contains helper code to time SQL statements.
    /// </summary>
    public class SqlProfiler
    {
        /// <summary>
        /// Returns a new <c>SqlProfiler</c> to be used in the <paramref name="profiler"/> session.
        /// </summary>
        public SqlProfiler(MiniProfiler profiler)
        {
            Profiler = profiler;
        }

        private readonly ConcurrentDictionary<Tuple<object, SqlExecuteType>, SqlTiming> _inProgress = new ConcurrentDictionary<Tuple<object, SqlExecuteType>, SqlTiming>();

        private readonly ConcurrentDictionary<IDataReader, SqlTiming> _inProgressReaders = new ConcurrentDictionary<IDataReader, SqlTiming>();

        /// <summary>
        /// Gets the profiling session this <c>SqlProfiler</c> is part of.
        /// </summary>
        public MiniProfiler Profiler { get; private set; }

        /// <summary>
        /// Tracks when 'command' is started.
        /// </summary>
        public void ExecuteStartImpl(ProfiledDbCommand command, SqlExecuteType type)
        {
            var id = Tuple.Create((object)command, type);
            var sqlTiming = new SqlTiming(command, type, Profiler);

            _inProgress[id] = sqlTiming;
        }

        /// <summary>
        /// Finishes profiling for 'command', recording durations.
        /// </summary>
        public void ExecuteFinishImpl(ProfiledDbCommand command, SqlExecuteType type, DbDataReader reader = null)
        {
            var id = Tuple.Create((object)command, type);
            var current = _inProgress[id];
            current.ExecutionComplete(reader != null, reader);
            SqlTiming ignore;
            _inProgress.TryRemove(id, out ignore);
            if (reader != null)
            {
                _inProgressReaders[reader] = current;
            }
        }

        /// <summary>
        /// Finishes profiling for 'command', recording durations.
        /// </summary>
        public void ExceptionImpl(ProfiledDbCommand command, SqlExecuteType type, Exception exception)
        {
            var id = Tuple.Create((object)command, type);
            var current = _inProgress[id];
            current.Exception(exception);
            SqlTiming ignore;
            _inProgress.TryRemove(id, out ignore);
            
        }

        /// <summary>
        /// Called when 'reader' finishes its iterations and is closed.
        /// </summary>
        public void ReaderFinishedImpl(IDataReader reader)
        {
            SqlTiming stat;

            // this reader may have been disposed/closed by reader code, not by our using()
            if (_inProgressReaders.TryGetValue(reader, out stat))
            {
                stat.ReaderFetchComplete(reader);
                SqlTiming ignore;
                _inProgressReaders.TryRemove(reader, out ignore);
            }
        }

        /// <summary>
        /// Returns all currently open commands on this connection
        /// </summary>
        public SqlTiming[] GetInProgressCommands()
        {
            return _inProgress.Values.OrderBy(x => x.StartMilliseconds).ToArray();
        }
    }

    /// <summary>
    /// Helper methods that allow operation on <c>SqlProfilers</c>, regardless of their instantiation.
    /// </summary>
    public static class SqlProfilerExtensions
    {
        /// <summary>
        /// Tracks when 'command' is started.
        /// </summary>
        public static void ExecuteStart(this SqlProfiler sqlProfiler, ProfiledDbCommand command, SqlExecuteType type)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ExecuteStartImpl(command, type);
        }

        /// <summary>
        /// Finishes profiling for 'command', recording durations.
        /// </summary>
        public static void ExecuteFinish(this SqlProfiler sqlProfiler, ProfiledDbCommand command, SqlExecuteType type, DbDataReader reader = null)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ExecuteFinishImpl(command, type, reader);
        }

        /// <summary>
        /// Called when 'reader' finishes its iterations and is closed.
        /// </summary>
        public static void ReaderFinish(this SqlProfiler sqlProfiler, IDataReader reader)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ReaderFinishedImpl(reader);
        }

        /// <summary>
        /// Called when 'reader' finishes its iterations and is closed.
        /// </summary>
        public static void Exception(this SqlProfiler sqlProfiler, ProfiledDbCommand command, SqlExecuteType type, Exception exception)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ExceptionImpl( command,  type, exception);
        }
    }
}