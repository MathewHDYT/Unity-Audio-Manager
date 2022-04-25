using AudioManager.Settings;
using NUnit.Framework;

public class TestShowIfAttribute {
    string[] m_conditions;
    ConditionOperator m_conditionOperator;
    ActionOnConditionFail m_actionOnConditionFail;

    [SetUp]
    public void TestSetUp() {
        m_conditions = new string[] { "" };
        m_conditionOperator = ConditionOperator.AND;
        m_actionOnConditionFail = ActionOnConditionFail.DO_NOT_DRAW;
    }

    [Test]
    public void TestConstructor() {
        ShowIfAttribute showIfAttribute = new ShowIfAttribute(m_actionOnConditionFail, m_conditionOperator, m_conditions);
        Assert.AreEqual(m_actionOnConditionFail, showIfAttribute.Action);
        Assert.AreEqual(m_conditionOperator, showIfAttribute.Operator);
        Assert.AreEqual(m_conditions, showIfAttribute.Conditions);
    }
}
