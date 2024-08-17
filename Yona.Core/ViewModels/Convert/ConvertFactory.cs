using Microsoft.Extensions.Logging;
using Yona.Core.Audio;
using Yona.Core.Projects.Builders;

namespace Yona.Core.ViewModels.Convert;

public class ConvertFactory(ConvertProjectBuilder builder, EncoderRepository encoders, LoopService loops, ILogger<ConvertViewModel> log)
{
    public ConvertViewModel Create(string[] files)
        => new(builder, encoders, loops, files, log);
}
