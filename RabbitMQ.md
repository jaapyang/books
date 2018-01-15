## RabbitMQ 学习笔记

### 参考资料
- [RabbitMq详解](http://www.cnblogs.com/ityouknow/p/6120544.html)

### 术语解释

通常我们谈到队列服务, 会有三个概念： 发消息者、队列、收消息者，RabbitMQ 在这个基本概念之上, 多做了一层抽象, 在发消息者和 队列之间, 加入了交换器 (Exchange). 这样发消息者和队列就没有直接联系, 转而变成发消息者把消息给交换器, 交换器根据调度策略再把消息再给队列。

#### 交换机(Exchange)

Exchange类似于数据通信网络中的交换机，提供消息路由策略。rabbitmq中，producer不是通过信道直接将消息发送给queue，而是先发送给Exchange。一个Exchange可以和多个Queue进行绑定，producer在传递消息的时候，会传递一个ROUTING_KEY，Exchange会根据这个ROUTING_KEY按照特定的路由算法，将消息路由给指定的queue。和Queue一样，Exchange也可设置为持久化，临时或者自动删除。

交换机的功能主要是接收消息并且转发到绑定的队列，交换机不存储消息，在启用ack模式后，交换机找不到队列会返回错误。交换机有四种类型：Direct, topic, Headers and Fanout

- __Direct(直接交换器)__：
    直接交换器，工作方式类似于单播，Exchange会将消息发送完全匹配ROUTING_KEY的Queue

    direct 类型的行为是"先匹配, 再投送". 即在绑定时设定一个 ```routing_key```, 消息的 ```routing_key``` 匹配时, 才会被交换器投送到绑定的队列中去.

    
- __Topic(主题交换器)__：
    按规则转发消息（最灵活）
    主题交换器，工作方式类似于组播，Exchange会将消息转发和ROUTING_KEY匹配模式相同的所有队列，比如，ROUTING_KEY为```user.stock```的Message会转发给绑定匹配模式为 ```*.stock```,```user.stock```， ```*.*``` 和```#.user.stock.#```的队列。（ * 表是匹配一个任意词组，#表示匹配0个或多个词组）

- __Headers__：设置header attribute参数类型的交换机

    消息体的header匹配（ignore）

- __Fanout(广播式交换器)__：转发消息到所有绑定队列
    广播式交换器，不管消息的ROUTING_KEY设置为什么，Exchange都会将消息转发给所有绑定的Queue。

#### producer&Consumer

producer指的是消息生产者，consumer消息的消费者

#### Queue
消息队列，提供了FIFO的处理机制，具有缓存消息的能力。rabbitmq中，队列消息可以设置为持久化，临时或者自动删除。
1. 设置为持久化的队列，queue中的消息会在server本地硬盘存储一份，防止系统crash，数据丢失
2. 设置为临时队列，queue中的数据在系统重启之后就会丢失
3. 设置为自动删除的队列，当不存在用户连接到server，队列中的数据会被自动删除


### 应用场景

#### 1. 单发送单接收
![](https://www.rabbitmq.com/img/tutorials/python-one.png)

#### 2. 单发送多接收

__使用场景__：一个发送端，多个接收端，如分布式的任务派发。为了保证消息发送的可靠性，不丢失消息，使消息持久化了。同时为了防止接收端在处理消息时down掉，只有在消息处理完成后才发送ack消息。
![](https://www.rabbitmq.com/img/tutorials/python-two.png)
    

#### 3. Publish/Subscribe
__使用场景__：发布、订阅模式，发送端发送广播消息，多个接收端接收。
![](https://www.rabbitmq.com/img/tutorials/python-three.png)


#### 4. Routing
__使用场景__：发送端按routing key发送消息，不同的接收端按不同的routing key接收消息。
__接收端和场景3的区别__：在绑定queue和exchange的时候使用了routing key，即从该exchange上只接收routing key指定的消息。

![](https://www.rabbitmq.com/img/tutorials/python-four.png)


#### 5. Topics
__使用场景__：发送端不只按固定的routing key发送消息，而是按字符串“匹配”发送，接收端同样如此。
![](https://www.rabbitmq.com/img/tutorials/python-five.png)


#### 6. RPC
![](https://www.rabbitmq.com/img/tutorials/python-six.png)