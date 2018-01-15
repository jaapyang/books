## RabbitMQ 学习笔记


### 应用场景

#### 1. 单发送单接收
![](https://www.rabbitmq.com/img/tutorials/python-one.png)

#### 2. 单发送多接收
    使用场景：一个发送端，多个接收端，如分布式的任务派发。为了保证消息发送的可靠性，不丢失消息，使消息持久化了。同时为了防止接收端在处理消息时down掉，只有在消息处理完成后才发送ack消息。
    ![](https://www.rabbitmq.com/img/tutorials/python-two.png)
    

#### 3. Publish/Subscribe
![](https://www.rabbitmq.com/img/tutorials/python-three.png)


#### 4. Routing
![](https://www.rabbitmq.com/img/tutorials/python-four.png)


#### 5. Topics
![](https://www.rabbitmq.com/img/tutorials/python-five.png)


#### 6. RPC
![](https://www.rabbitmq.com/img/tutorials/python-six.png)