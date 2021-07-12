using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    TestClass origin;
    TestClass clone;

    [SetUp]
    public void SetUp()
    {
        origin = new TestClass();
    }
    private void Clone()
    {
        clone = (TestClass)origin.DeepClone();
    }
    [Test]
    public void DeepClone_Test()
    {
        Clone();
        Assert.AreNotEqual(origin, clone);
    }
    [Test]
    public void DeepClone_TestString()
    {
        var originValue = "Test";
        origin.StringValue = originValue;
        Clone();
        var cloneValue = "";
        clone.StringValue = cloneValue;
        Assert.AreNotEqual(origin.StringValue, clone.StringValue);
    }

    [Test]
    public void DeepClone_TestClass()
    {
        var originValue = new TestObject() { value=100};
        origin.TObject= originValue;
        Clone();

        Assert.AreNotEqual(origin.TObject, clone.TObject);
    }

    [Test]
    public void DeepClone_TestClassValue()
    {
        var originValue = new TestObject() { value = 100 };
        origin.TObject = originValue;
        Clone();
        clone.TObject.value = 30;
        Assert.AreNotEqual(origin.TObject.value, clone.TObject.value);
    }
    [Test]
    public void DeepClone_TestList()
    {
        var originValue = new List<TestObject>();
        origin.list = originValue;

        for (int i = 0; i < 100; i++)
        {
            origin.list.Add(new TestObject() { value=i});
        }
        Clone();
        Assert.AreNotEqual(origin.list,clone.list);
    }


    [Test]
    public void DeepClone_TestListContext()
    {
        var originValue = new List<TestObject>();
        origin.list = originValue;

        for (int i = 0; i < 100; i++)
        {
            origin.list.Add(new TestObject() { value = i });
        }
        Clone();
        for (int i = 0; i < origin.list.Count; i++)
        {
            Assert.AreNotEqual(origin.list[i], clone.list[i]);
        }
    }
    [Test]
    public void DeepClone_TestListContextValue()
    {
        var originValue = new List<TestObject>();
        origin.list = originValue;

        for (int i = 0; i < 100; i++)
        {
            origin.list.Add(new TestObject() { value = i });
        }

        Clone();
        for (int i = 0; i < 100; i++)
        {
            clone.list[i]=new TestObject() { value = i+1 };
        }
        for (int i = 0; i < origin.list.Count; i++)
        {
            Assert.AreNotEqual(origin.list[i].value, clone.list[i].value);
        }
    }




}
[Serializable]
public class TestObject
{
    public int value;
}

[Serializable]
public class TestClass
{
    public string StringValue { get; set; }
    public int age { get; set; }

    public TestObject TObject;
    public List<TestObject> list;
    public object DeepClone()
    {

        using (Stream objectStream = new MemoryStream())
        {
            //序列化物件格式
            IFormatter formatter = new BinaryFormatter();
            //將自己所有資料序列化
            formatter.Serialize(objectStream, this);
            //複寫資料流位置，返回最前端
            objectStream.Seek(0, SeekOrigin.Begin);
            //再將objectStream反序列化回去 
            return formatter.Deserialize(objectStream) as TestClass;
        }


    }
}
