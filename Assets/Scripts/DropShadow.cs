using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BRW.UI
{
    /// <summary>
    /// Modified script for DropShadow. Initial script : https://pastebin.com/YGeaZwG3 
    /// </summary>
    public class DropShadow : BaseMeshEffect
    {
        [SerializeField] private Color _shadowColor = new Color(0f, 0f, 0f, 0.5f);
        [SerializeField] private Vector2 _shadowDistance = new Vector2(1f, -1f);
        [SerializeField] private bool _m_UseGraphicAlpha = true;
        [SerializeField] private int _iterations = 5;
        [SerializeField] private Vector2 _shadowSpread = Vector2.one;

        protected DropShadow()
        { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            EffectDistance = _shadowDistance;
            base.OnValidate();
        }

#endif

        public Color effectColor 
        { 
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 ShadowSpread
        {
            get => _shadowSpread;
            set
            {
                _shadowSpread = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public int Iterations
        {
            get { return _iterations; }
            set
            {
                _iterations = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public Vector2 EffectDistance
        {
            get => _shadowDistance;
            set
            {
                _shadowDistance = value;

                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public bool UseGraphicAlpha
        {
            get => _m_UseGraphicAlpha;
            set
            {
                _m_UseGraphicAlpha = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }


        void DropShadowEffect(List<UIVertex> verts)
        {
            UIVertex vt;
            int count = verts.Count;

            List<UIVertex> vertsCopy = new List<UIVertex>(verts);
            verts.Clear();

            for (int i = 0; i < _iterations; i++)
            {
                for (int v = 0; v < count; v++)
                {
                    vt = vertsCopy[v];
                    Vector3 position = vt.position;
                    float fac = (float)i / (float)_iterations;
                    position.x *= (1 + _shadowSpread.x * fac * 0.01f);
                    position.y *= (1 + _shadowSpread.y * fac * 0.01f);
                    position.x += _shadowDistance.x * fac;
                    position.y += _shadowDistance.y * fac;
                    vt.position = position;
                    Color32 color = _shadowColor;
                    color.a = (byte)((float)color.a / (float)_iterations);
                    vt.color = color;
                    verts.Add(vt);
                }
            }

            for (int i = 0; i < vertsCopy.Count; i++)
            {
                verts.Add(vertsCopy[i]);
            }
        }
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            List<UIVertex> output = new List<UIVertex>();
            vh.GetUIVertexStream(output);

            DropShadowEffect(output);

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
        }
    }
}
