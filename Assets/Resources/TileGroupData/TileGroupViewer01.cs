using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Reflection;
using System;

public class TileGroupViewer01 : OdinAttributeProcessor<TileGroupData>
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        base.ProcessChildMemberAttributes(parentProperty, member, attributes);

        /*  if(member.MemberType == MemberTypes.Field)
          {

          }*/
        //attributes.Add(new LabelWidthAttribute(60));
      /*  if(member.Name == "tiles")
        {
            //Debug.Log($"{member}");
            var baseClass = (TileGroupData)parentProperty.ValueEntry.WeakSmartValue;

            int num = baseClass.tiles.Length;
            Debug.Log($"{num}");
            //member.
        }*/
    }
}

/*
public class TileGroupViewer02 : OdinAttributeProcessor<TileList>
{
    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        base.ProcessChildMemberAttributes(parentProperty, member, attributes);

        attributes.Add(new BoxGroupAttribute("Tiles 2"));
        attributes.Add(new HideLabelAttribute());
        if (member.Name == "tiles")
        {
            attributes.Add(new PreviewFieldAttribute(60, ObjectFieldAlignment.Left));
        }

        //Debug.Log($"field: {member.Name}");
    }
}*/