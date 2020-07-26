using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Attributes;
using static Assets.Scripts.Attributes.Attributes;

public class DebugMenu : MonoBehaviour
{
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;

        transform.Find("DebugCanvas/incrVigor").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Vigor));
        transform.Find("DebugCanvas/decrVigor").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Vigor));

        transform.Find("DebugCanvas/incrSpirit").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Spirit));
        transform.Find("DebugCanvas/decrSpirit").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Spirit));

        transform.Find("DebugCanvas/incrStrength").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Strength));
        transform.Find("DebugCanvas/decrStrength").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Strength));

        transform.Find("DebugCanvas/incrIntelligence").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Intelligence));
        transform.Find("DebugCanvas/decrIntelligence").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Intelligence));

        transform.Find("DebugCanvas/incrResilience").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Resilience));
        transform.Find("DebugCanvas/decrResilience").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Resilience));

        transform.Find("DebugCanvas/incrVitality").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Vitality));
        transform.Find("DebugCanvas/decrVitality").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Vitality));

        transform.Find("DebugCanvas/incrFocus").GetComponent<Button>().onClick.AddListener(() => attributes.IncreaseAttribute(AttributeType.Focus));
        transform.Find("DebugCanvas/decrFocus").GetComponent<Button>().onClick.AddListener(() => attributes.DecreaseAttribute(AttributeType.Focus));

        transform.Find("DebugCanvas/addSouls").GetComponent<Button>().onClick.AddListener(() => attributes.SetSouls(attributes.GetSouls() + 100000));
        transform.Find("DebugCanvas/maxLevel").GetComponent<Button>().onClick.AddListener(MaxAllAttributes);

        attributes.OnAttributeChanged += Attributes_OnAttributesChanged;
    }

    private void Attributes_OnAttributesChanged(object sender, EventArgs e)
    {
        UpdateStatisticsVisuals();
    }

    private void UpdateStatisticsVisuals()
    {
        transform.Find("DebugCanvas/textVigor").GetComponent<Text>().text = "Vigor: " + _attributes.GetAttributeAmount(AttributeType.Vigor);
        transform.Find("DebugCanvas/textSpirit").GetComponent<Text>().text = "Spirit: " + _attributes.GetAttributeAmount(AttributeType.Spirit);
        transform.Find("DebugCanvas/textStrength").GetComponent<Text>().text = "Strength: " + _attributes.GetAttributeAmount(AttributeType.Strength);
        transform.Find("DebugCanvas/textIntelligence").GetComponent<Text>().text = "Intelligence: " + _attributes.GetAttributeAmount(AttributeType.Intelligence);
        transform.Find("DebugCanvas/textResilience").GetComponent<Text>().text = "Resilience: " + _attributes.GetAttributeAmount(AttributeType.Resilience);
        transform.Find("DebugCanvas/textVitality").GetComponent<Text>().text = "Vitality: " + _attributes.GetAttributeAmount(AttributeType.Vitality);
        transform.Find("DebugCanvas/textFocus").GetComponent<Text>().text = "Focus: " + _attributes.GetAttributeAmount(AttributeType.Focus);
    }

    private void MaxAllAttributes()
    {
        var attributeCollection = Enum.GetValues(typeof(AttributeType));

        foreach (var attribute in attributeCollection)
        {
            _attributes.SetAttribute((AttributeType) attribute, 99);
        }
    }
}
