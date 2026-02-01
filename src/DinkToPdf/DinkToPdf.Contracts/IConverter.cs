using System;
using DinkToPdf.EventDefinitions;

namespace DinkToPdf.Contracts;

public interface IConverter
{
	event EventHandler<PhaseChangedArgs> PhaseChanged;

	event EventHandler<ProgressChangedArgs> ProgressChanged;

	event EventHandler<FinishedArgs> Finished;

	event EventHandler<ErrorArgs> Error;

	event EventHandler<WarningArgs> Warning;

	byte[] Convert(IDocument document);
}
