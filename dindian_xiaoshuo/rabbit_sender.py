#! /usr/bin/env python3
# -*-coding:utf-8-*-


import pika


class RabbitMqMessageHandler:
    def __init__(self):
        pass

    @staticmethod
    def sent_message(host, queue_name, routing_key_name, message_content, exchange_name=""):
        connection = pika.BlockingConnection(pika.ConnectionParameters(host))
        channel = connection.channel()
        channel.queue_declare(queue=queue_name)
        channel.basic_publish(exchange=exchange_name,
                              routing_key=routing_key_name,
                              body=message_content)
        connection.close()

    @staticmethod
    def send_work_message(host, exchange, queue, route_key, message_body):
        connecton = pika.BlockingConnection(pika.ConnectionParameters(host=host))
        channel = connecton.channel()
        channel.queue_declare(queue=queue, durable=True)
        # message = json.dumps(message_body.__dict__, default=lambda x: x.__dict__)
        channel.basic_publish(exchange=exchange,
                              routing_key=route_key,
                              body=message_body,
                              properties=pika.BasicProperties(
                                  delivery_mode=2,  # make message persistent
                              ))
        print(" [x] sent message")
        connecton.close()

    @staticmethod
    def receive_message(host, queue_name, callback):
        connection = pika.BlockingConnection(pika.ConnectionParameters(host))
        channel = connection.channel()
        channel.queue_declare(queue_name)

        # def receive_callback(ch, method, propertyies, body):
        #     print("received message....")
        #     message_content = str(body, encoding='utf-8')
        #     from Handlers.book_download_handler import BookDownLoadHandler
        #     handler = BookDownLoadHandler()
        #     handler.handler_received_menu_download_task(message_content)
        #     # print("Received %r" % body)

        channel.basic_consume(callback,
                              queue=queue_name,
                              no_ack=True)

        print(' [*] waiting for message. to exists press CTRL+C')
        channel.start_consuming()
