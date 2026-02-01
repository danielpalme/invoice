using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DinkToPdf.Contracts;

namespace DinkToPdf;

public class SynchronizedConverter : BasicConverter
{
	private Thread conversionThread;

	private BlockingCollection<Task> conversions = new BlockingCollection<Task>();

	private bool kill;

	private readonly object startLock = new object();

	public SynchronizedConverter(ITools tools)
		: base(tools)
	{
	}

	public override byte[] Convert(IDocument document)
	{
		return Invoke(() => base.Convert(document));
	}

	public TResult Invoke<TResult>(Func<TResult> @delegate)
	{
		StartThread();
		Task<TResult> task = new Task<TResult>(@delegate);
		lock (task)
		{
			conversions.Add(task);
			Monitor.Wait(task);
		}
		if (task.Exception != null)
		{
			throw task.Exception;
		}
		return task.Result;
	}

	private void StartThread()
	{
		lock (startLock)
		{
			if (conversionThread == null)
			{
				conversionThread = new Thread(Run)
				{
					IsBackground = true,
					Name = "wkhtmltopdf worker thread"
				};
				kill = false;
				conversionThread.Start();
			}
		}
	}

	private void StopThread()
	{
		lock (startLock)
		{
			if (conversionThread != null)
			{
				kill = true;
				while (conversionThread.ThreadState == ThreadState.Stopped)
				{
				}
				conversionThread = null;
			}
		}
	}

	private void Run()
	{
		while (!kill)
		{
			Task task = conversions.Take();
			lock (task)
			{
				task.RunSynchronously();
				Monitor.Pulse(task);
			}
		}
	}
}
