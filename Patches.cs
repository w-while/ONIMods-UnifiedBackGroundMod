using Delaunay.Geo;
using HarmonyLib;
using Klei;
using PeterHan.PLib.Options;
using ProcGen;
using System;
using UnityEngine;


namespace UnifiedBackGroundMod
{
    public class Patches
    {
        [HarmonyPatch(typeof(SubworldZoneRenderData), nameof(SubworldZoneRenderData.OnActiveWorldChanged))]
        public class SubworldZoneRenderData_OnActiveWorldChanged_Patch
        {
            static bool Prefix(ref Texture2D ___colourTex, ref Texture2D ___indexTex, Color32[] ___zoneColours, ref int[] ___zoneTextureArrayIndices)
            {
                byte[] rawTextureData = ___colourTex.GetRawTextureData();
                byte[] rawTextureData2 = ___indexTex.GetRawTextureData();
                WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
                Vector2 zero = Vector2.zero;
                for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
                {
                    WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
                    Polygon poly = overworldCell.poly;
                    zero.y = (int)Mathf.Floor(poly.bounds.yMin);
                    while (zero.y < Mathf.Ceil(poly.bounds.yMax))
                    {
                        zero.x = (int)Mathf.Floor(poly.bounds.xMin);
                        while (zero.x < Mathf.Ceil(poly.bounds.xMax))
                        {
                            if (poly.Contains(zero))
                            {
                                int num = Grid.XYToCell((int)zero.x, (int)zero.y);
                                if (Grid.IsValidCell(num))
                                {
                                    if (Grid.IsActiveWorld(num))
                                    {
                                        if (SingletonOptions<Settings>.Instance.TargetBackGround==BG.SpaceBackGround)
                                        {
                                            rawTextureData2[num]=byte.MaxValue;
                                        }
                                        else if ((int)SingletonOptions<Settings>.Instance.TargetBackGround>=___zoneTextureArrayIndices.Length)
                                        {
                                            rawTextureData2[num] = byte.MaxValue;
                                        }
                                        else
                                        {
                                            rawTextureData2[num] = ((overworldCell.zoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)___zoneTextureArrayIndices[(int)SingletonOptions<Settings>.Instance.TargetBackGround]));
                                        }
                                        Color32 color;
                                        color = ___zoneColours[(int)overworldCell.zoneType];
                                        if (SingletonOptions<Settings>.Instance.ShouldDisableZoneColor&&overworldCell.zoneType != SubWorld.ZoneType.Space)
                                        {
                                            color = new Color32(220, 220, 220, 0);
                                        }
                                        rawTextureData[num * 3] = color.r;
                                        rawTextureData[num * 3 + 1] = color.g;
                                        rawTextureData[num * 3 + 2] = color.b;
                                    }
                                    else
                                    {
                                        rawTextureData2[num] = byte.MaxValue;
                                        Color32 color2 = ___zoneColours[7];
                                        rawTextureData[num * 3] = color2.r;
                                        rawTextureData[num * 3 + 1] = color2.g;
                                        rawTextureData[num * 3 + 2] = color2.b;
                                    }
                                }
                            }
                            zero.x += 1f;
                        }
                        zero.y += 1f;
                    }
                }
                ___colourTex.LoadRawTextureData(rawTextureData);
                ___indexTex.LoadRawTextureData(rawTextureData2);
                ___colourTex.Apply();
                ___indexTex.Apply();
                Shader.SetGlobalTexture("_WorldZoneTex", ___colourTex);
                Shader.SetGlobalTexture("_WorldZoneIndexTex", ___indexTex);

                return false;//原函数不运行
            }
        }
    }
}
