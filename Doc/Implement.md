# 实现思路

# 闭包
通过Slot对每个变量生命期进行转移

# Delegate
声明函数作为占位符, 再通过注册实现代码

# Package

函数注册时, 将函数注册到方法区, 函数不再添加到常量表, 无需指令集

将函数调用时, Func的Expr进行分析

* 本地函数调用
	从本package的constant中加载函数
	
* 包.函数调用
	从其他package的constant中加载函数体

* 动态调用
	从本package的constant中加载函数

