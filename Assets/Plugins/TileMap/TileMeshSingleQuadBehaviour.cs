﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTileMap
{
    /// <summary>
    /// The mesh behaviour used for MeshMode.SingleQuad.
    /// </summary>
    public class TileMeshSingleQuadBehaviour : TileMeshBehaviour
    {
        private Texture2D m_texture;
        private bool m_textureDirty;
        private bool m_bufferDirty;
        int[] places;
        private List<BufferedTex> texBuffer;

        public override TileMeshSettings Settings
        {
            get { return base.Settings; }
            set
            {
                // TODO a bit copy and paste code, but we only want to recreate the texture if settings changed
                if (value == null)
                    throw new ArgumentNullException("value");
                if (base.Settings != null && base.Settings.Equals(value))
                    return;

                bool resolutionChanged = false;

                if (base.Settings != null)
                {
                    resolutionChanged = base.Settings.TileResolution != value.TileResolution;
                }

                base.Settings = value;

                CreateTexture(! resolutionChanged);
            }
        }

        private void LateUpdate()
        {
            if (m_textureDirty && m_texture != null)
            {
                m_texture.Apply();
                m_textureDirty = false;
            }
            if (m_bufferDirty)
            {
                BufferedTex b = new BufferedTex();
                b.tex = m_texture;
                b.places = places;
                texBuffer.Add(b);
            }
        }

        public override void SetTile(int x, int y, Sprite sprite)
        {
            var rect = sprite.rect;
            var colors = sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            SetTile(x, y, colors);
        }

        /// <summary>
        /// Paint a tile with a solid color.
        /// </summary>
        public void SetTile(int x, int y, Color color)
        {
            var colors = Enumerable.Repeat(color, base.Settings.TileResolution * base.Settings.TileResolution).ToArray();
            SetTile(x, y, colors);
        }

        /// <summary>
        /// Paint a tile with custom colors, usually a sprite.
        /// </summary>
        private void SetTile(int x, int y, Color[] colors)
        {
            if (m_texture == null)
            {
                Debug.LogError("Texture has not been created");
                return;
            }

            var resolution = base.Settings.TileResolution;

            // the texture has 0,0 in the bottom left, flip y to put it at upper left
            m_texture.SetPixels(
                x * resolution,
                y * resolution,
                resolution,
                resolution,
                colors);

            if (Application.isPlaying)
                m_textureDirty = true;
            else
                m_texture.Apply();
        }

        protected override Mesh CreateMesh()
        {
            var vertices = new Vector3[4];
            var triangles = new int[6];
            var normals = new Vector3[4];
            var uv = new Vector2[4];
            float sizeX = base.Settings.TilesX * base.Settings.TileSize;
            float sizeY = base.Settings.TilesY * base.Settings.TileSize;

            // vertices going clockwise
            // 2--3
            // | /|
            // |/ |
            // 0--1
            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(sizeX, 0, 0);
            vertices[2] = new Vector3(0, sizeY, 0);
            vertices[3] = new Vector3(sizeX, sizeY, 0);

            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 3;

            triangles[3] = 0;
            triangles[4] = 3;
            triangles[5] = 1;

            normals[0] = Vector3.forward;
            normals[1] = Vector3.forward;
            normals[2] = Vector3.forward;
            normals[3] = Vector3.forward;

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);

            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                normals = normals,
                uv = uv,
                name = "TileMapMesh"
            };
            return mesh;
        }

        private void CreateTexture(bool keepData = true)
        {
            Texture2D texture = new Texture2D(
                base.Settings.TilesX * base.Settings.TileResolution,
                base.Settings.TilesY * base.Settings.TileResolution,
                base.Settings.TextureFormat,
                false);
            texture.name = "TileMapTexture";
            texture.filterMode = base.Settings.TextureFilterMode;
            texture.wrapMode = TextureWrapMode.Clamp;

            if (m_texture && keepData)
            {
                int width = Mathf.Clamp(m_texture.width, 0, texture.width);
                int height = Mathf.Clamp(m_texture.height, 0, texture.height);

                Color[] colors = m_texture.GetPixels(0, 0, width, height);

                texture.SetPixels(0, 0, width, height, colors);

                texture.Apply();
            }

            m_texture = texture;

            MaterialTexture = m_texture;
        }

        public override void AnimateMesh(TileAnim[] tileAnims, Grid<Sprite> tiles)
        {
            if (texBuffer == null)
            {
                texBuffer = new List<BufferedTex>();
            }
            Texture2D tex = default(Texture2D);
            places = new int[tileAnims.Length];
            for (int i = 0; i < tileAnims.Length; i++)
            {
                places[i] = tileAnims[i].place;
            }
            for (int i = 0; i < texBuffer.Count; i++)
            {
                if (texBuffer[i].places.SequenceEqual(places))
                {
                    m_texture = texBuffer[i].tex;
                    MaterialTexture = m_texture;
                    tex = m_texture;
                    break;
                }
            }
            if (tex == null)
            {
                CreateTexture();
                for (int y = 0; y < tiles.SizeY; y++)
                {
                    for (int x = 0; x < tiles.SizeX; x++)
                    {
                        SetTile(x, y, tiles[x, y]);
                    }
                }
                m_bufferDirty = true;
            }
        }
    }
}
