#! /usr/bin/env python3
# -*-coding:utf-8-*-
import json
import logging
import os
import random
import requests
import time
from bs4 import BeautifulSoup

from rabbit_sender import RabbitMqMessageHandler

LIST_URL = 'http://www.23us.so/files/article/html/0/240/index.html'
SAVE_DIR = 'D:\\download_books'
SLEEP_TIME = 3
MENU_HTML = ''
http_proxies_list = [
    # {"http": "http://115.28.148.137:8118", "https": "https://112.114.99.133:8118"},
    {"http": "http://106.75.56.87:80", "https": "https://123.53.137.32:48420"},
    # {"http": "http://221.233.85.18:3128", "https": "https://118.72.124.97:80"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
    # {"http": "http://xxxx:xxx", "https": "https://xxxx:8118"},
]

headers_list = [
    {
        "User-Agent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36"},
]


class Charector():
    def __init__(self, url, title):
        self.Href = url
        self.Title = title


def random_get_the_first_from_list(data_list):
    random.shuffle(data_list)
    data = data_list[0]
    # print(data)
    return data


def get_page_html(url):
    """获取指定URL页面的HTML"""
    time.sleep(SLEEP_TIME)
    # proxies = random_get_the_first_from_list(http_proxies_list)
    headers = random_get_the_first_from_list(headers_list)
    with requests.get(url, headers=headers) as response:
        # response = requests.get(url, proxies=proxies, headers=headers)
        return response.content.decode('utf-8')


def parse_list(menu_html):
    """获取小说目录列表"""
    # print(menu_html)
    soup = BeautifulSoup(menu_html)
    link_list = soup.find(id='at').find_all('a')
    # print(link_list)
    detail_url_list = []
    for link in link_list:
        c = Charector(link.attrs['href'], link.text)
        detail_url_list.append(c)
    # print(str(len(detail_url_list)))
    return detail_url_list


def parse_detail_page_content(page_content):
    """
    解析小说页内容
    """
    soup = BeautifulSoup(page_content)
    content = soup.find(id='contents').text
    return content


def read_txt_content_by_line(path):
    with open(path, 'r', encoding='utf-8') as f:
        return f.readlines()


def receive_book_html_parse_callback(ch, method, propertyies, body):
    print("received book_html....")

    message_content = str(body, encoding='utf-8')
    charact_list = parse_list(message_content)

    file_name = input("请输入保存的文件名:")
    start_chapterId = input("请输入起始章节:")

    logging.basicConfig(level=logging.INFO,
                        format='%(levelname)s \t %(asctime)s ---------- \n\t%(message)s\n',
                        datefmt='%a, %d %b %Y %H:%M:%S',
                        filename=os.path.join(SAVE_DIR, 'log', file_name + '.log'),
                        filemode='w')

    with open(os.path.join(SAVE_DIR, file_name + '.txt'), 'w', encoding='utf-8') as f:
        index = 0
        chapter_id = 1

        for item in charact_list:
            chapter_id = chapter_id + 1
            print(chapter_id)
            if chapter_id < int(start_chapterId):
                continue

            index = index + 1

            if index > 30:
                SLEEP_TIME = 60
                index = 0
            else:
                SLEEP_TIME = random.randint(1, 10)

            running_message = item.Href + '/' + item.Title + '\tindex:' + str(index) + '\tSleep:' + str(SLEEP_TIME)
            print(running_message)
            logging.info(running_message)
            try:
                page_html = get_page_html(item.Href)
                page_content = parse_detail_page_content(page_html)
                f.write("\n")
                f.write(item.Title)
                f.write("\n")
                f.write(page_content)
            except BaseException as e:
                logging.error(e)
                print(e)
    input("下载结束，请手动关闭.....")


if __name__ == '__main__':
    """测试动态代理"""
    # content = get_page_html("http://httpbin.org/headers")
    # print(content)

    """输出已经下载的小说内容"""
    # lines = read_txt_content_by_line(os.path.join(SAVE_DIR, '巫师之旅.txt'))
    # for line in lines:
    #     print(line)

    """下载小说"""

    # LIST_URL = input("请输入列表页网址:")

    RabbitMqMessageHandler.receive_message(host="localhost",
                                           queue_name="parse_menu_html",
                                           callback=receive_book_html_parse_callback)

    # menu_html = get_page_html(LIST_URL)


    """测试分章节下载小说"""
    # html = get_page_html('http://www.23us.so/files/article/html/14/14741/9528946.html')
    # content = parse_detail_page_content(html)
    # save_to_file('01 test',content)
