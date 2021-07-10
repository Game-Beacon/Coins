using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Tools;
using System.Xml;

//public class BattleTest
//{
//    private Fakecharactor fakecharactor;
//    [SetUp]
//    public void Setup()
//    {
//        fakecharactor = SetupCharacter();
//    }
//    [Test]
//    [TestCase(100,0)]
//    [TestCase(-100,100)]
//    [TestCase(200,0)]
//    public void MinusHp_Attack_ValueChange(int damage, int expectedHp)
//    {
//        fakecharactor.MinusHp(damage);
//        Assert.AreEqual(expectedHp,fakecharactor.Hp);
//    }
    
//    [Test]
//    [TestCase(100,true)]
//    [TestCase(80,false)]
//    [TestCase(-80,false)]
//    public void MinusHp_Attack_ChangeIsDead(int damage, bool dead)
//    {
//        fakecharactor.MinusHp(damage);
//        Assert.AreEqual(dead, fakecharactor.IsDead);
//    }

//    private static Fakecharactor SetupCharacter()
//    {
//        Fakecharactor fakecharactor = new Fakecharactor();
//        return fakecharactor;
//    }
//}
