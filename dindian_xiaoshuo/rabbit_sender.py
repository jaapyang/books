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
