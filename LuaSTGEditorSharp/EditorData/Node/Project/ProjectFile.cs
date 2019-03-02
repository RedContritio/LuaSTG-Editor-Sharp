﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Lua;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Project
{
    [Serializable, NodeIcon("images/16x16/patch.png")]
    [RequireAncestor(typeof(ProjectRoot))]
    [ClassNode]
    [LeafNode]
    [CreateInvoke(0), RCInvoke(0)]
    public class ProjectFile : TreeNode
    {
        [JsonConstructor]
        private ProjectFile() : base() { }

        public ProjectFile(DocumentData workSpaceData) 
            : base(workSpaceData) { attributes.Add(new AttrItem("Path", this, "lstgesFile")); }

        public ProjectFile(DocumentData workSpaceData, string code) 
            : base(workSpaceData) { attributes.Add(new AttrItem("Path", this, "lstgesFile") { AttrInput = code }); }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = "".PadLeft(spacing * 4);
            yield return sp + "Include\'" + StringParser.ParseLua(Path.GetFileNameWithoutExtension(NonMacrolize(0)) + ".lua") + "\'\n";
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
        }

        public override string ToString()
        {
            return "Include File: " + attributes[0].AttrInput;
        }

        public override MetaInfo GetMeta()
        {
            return new ProjectFileMetaInfo(this);
        }

        public override object Clone()
        {
            var n = new ProjectFile(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }
    }
}
