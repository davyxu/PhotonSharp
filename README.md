# PhotonSharp
这是一个参考golang源码写出的脚本引擎, 语言特性如下:

* 类C系的大括号block分隔

* var变量强制声明

* 无类型

* 类javascript的外置array及类golang的外置map, 没有lua的table通用容器

* 不以分号语句结尾, 表达式无需多加括号

整体设计目的

* 不一味追求速度, 但并不表示不优化
* 侧重通用性, 词法分析器可以拆离
* 依然坚持OO模拟更多的语言特性


语言一览
	
	// 函数调用
	func mul( a, b ){
	    return a * b
	}
	
	func foo( a, b ){
	    return a + mul( b, 2 )
	}
	
	var y = foo( 1, 2 )
	
	// 变量交换
	var x, y = 1, 2
	x, y = y, x
	
	// for数字循环
	var x = 10
	for i = 1;i < 3;i=i+1 {
	    x = x - 1
	}


	
# 备注

感觉不错请star, 谢谢!

博客: http://www.cppblog.com/sunicdavy

知乎: http://www.zhihu.com/people/xu-bo-62-87

邮箱: sunicdavy@qq.com
