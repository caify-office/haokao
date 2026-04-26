using System;

namespace HaoKao.LearnProgressService.Domain.Commands.LearnProgress;

/// <summary>
/// 更换视频进度
/// </summary>
/// <param name="VideoId">视频id</param>
/// <param name="Duration">时长</param>
public record UpdateThVideoProgressCommand(
    Guid VideoId,
    double Duration
) : Command("更换视频进度");