# PhotonSharp
一个结构易读的完整脚本系统

语言特性

* C系的大括号block分隔

* var变量声明

* 无类型

* 类javascript的外置array及类golang的外置map, 没有lua的table通用容器

* 不以分号语句结尾, 表达式无需多加括号

* 采用通用的词法分析器

整体设计目的

* 以易读性为前提, 不一味追求性能

* 强大的扩展性




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

	// 闭包
	func foo( ){
		var a = 2

		return func( x ){
			var y = 1
			return x + y + a
		}
	}

# 调试器
![调试器](ScreenShot/debugger.png)
	
# 备注

感觉不错请star, 谢谢!

博客: http://www.cppblog.com/sunicdavy

知乎: http://www.zhihu.com/people/xu-bo-62-87

邮箱: sunicdavy@qq.com
