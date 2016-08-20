﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTileMap
{
    struct BufferedTex
    {
        public int[] places;
        public Texture2D tex;
    }

    /// <summary>
    /// The base class for behaviours that holds and renders the actual mesh that is the TileMap.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class TileMeshBehaviour : MonoBehaviour
    {
        private bool m_initialized;
        private Material m_material;
        private TileMeshSettings m_settings;
        private Mesh m_mesh;

        public virtual TileMeshSettings Settings
        {
            get { return m_settings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (m_settings != null && m_settings.Equals(value))
                {
                    //Debug.Log("Settings equal, doing nothing");
                    return;
                }

                if (value.TilesX < 0)
                    throw new ArgumentException("TilesX cannot be less than zero");
                if (value.TilesY < 0)
                    throw new ArgumentException("TilesY cannot be less than zero");
                if (value.TileResolution < 0)
                    throw new ArgumentException("TilesResolution cannot be less than zero");
                if (value.TileSize < 0f)
                    throw new ArgumentException("TileSize cannot be less than zero");

                m_settings = value;

                if (m_material == null)
                {
                    m_material = new Material(Shader.Find("Sprites/ColorFlash"));
                }


                m_mesh = CreateMesh();

                MeshFilter meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = m_mesh;

                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.material = m_material;
            }
        }

        protected Material Material
        {
            get
            {
                if (m_material == null)
                {
                    m_material = new Material(Shader.Find("Sprites/Default"));
                }
                return m_material;
            }
        }

        protected Texture MaterialTexture
        {
            get { return Material.GetTexture("_MainTex"); }
            set { Material.SetTexture("_MainTex", value); }
        }

        public abstract void SetTile(int x, int y, Sprite sprite);

        public Rect GetTileBoundsLocal(int x, int y)
        {
            var size = m_settings.TileSize;
            return new Rect(
                x * size,
                y * size,
                size,
                size);
        }

        public Rect GetTileBoundsWorld(int x, int y)
        {
            var rect = GetTileBoundsLocal(x, y);
            var position = transform.position;
            rect.x += position.x;
            rect.y += position.y;
            return rect;
        }

        public void SetLayer (int l)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerID = l;
        }

        protected abstract Mesh CreateMesh();

        public abstract void AnimateMesh(TileAnim[] m_tileAnims, Grid<Sprite> tiles);
    }
}
