using Newtonsoft.Json;
using PeterHan.PLib.Options;


namespace UnifiedBackGroundMod
{
    public enum BG
    {
        FrozenWastes_BG0,
        CrystalCaverns_BG1,
        BoggyMarsh_BG2,
        Sandstone_BG3,
        ToxicJungle_BG4,
        MagmaCore_BG5,
        OilField_BG5,
        SpaceBackGround,
        Ocean_BG6,
        Rust_BG7,
        Forest_BG8,
        Radioactive_BG9,
        Swamp_BG10,
        Wasteland_BG11,
        RocketInterior_BG12,
        Metallic_BG7,
        Barren_BG3,
        Moo_BG13,
    }//和SubworldZoneRenderData.zoneTextureArrayIndices要对应上
    [RestartRequired]
    [JsonObject(MemberSerialization.OptIn)]//只有带[JsonProperty]的属性会序列化
    public class Settings
    {

        [Option("禁用不同生态的颜色滤镜\nDisable color filters for different biome", null)]
        [JsonProperty]
        public bool ShouldDisableZoneColor { get; set; } = false;

        [Option("所有背景都改为该生态的背景：\nChange all backgrounds to the target biome's background", "可以通过生态名字之后的序号参考MOD主页的图片找到具体的背景图片", null)]
        [JsonProperty]
        public BG TargetBackGround { get; set; } = BG.Sandstone_BG3;
    }
}
