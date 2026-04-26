namespace HaoKao.LiveBroadcastService.Domain.Enums;

public enum VideoType
{
    Unknown, // 未知格式

    // 常见的视频格式
    MP4, // MPEG-4 Part 14
    MOV, // QuickTime File Format
    MPEG, // MPEG Video
    avi, // Audio Video Interleave
    WMV, // Windows Media Video
    FLV, // Flash Video
    AVI, // Open Video Format (区分大小写，AVI通常指Windows AVI)
    WebM, // WebM Open web media
    MKV, // Matroska Video
    // 根据需要添加更多格式...

    // 高清和4K视频格式
    H264, // H.264/AVC
    HEVC, // H.265/HEVC
    VP9, // VP9

    // 其他专业或特殊用途的视频格式
    ProRes, // Apple ProRes
    Cineform, // GoPro Cineform (CineForm)
}