#!/usr/bin/env python
# coding=utf-8

"""
用于获取代理IP列表，并通过访问百度来验证该代理是否有效
"""
import logging
import ssl
import urllib
import http.cookiejar
import re
import socket

import time

import requests


class OpenUrl:
    def __init__(self):
        self.result = ""

    def openpage(self):  # url为相对路径
        try:
            url = "http://www.baidu.com"
            self.result = self.opener.open(url).read().decode("gb2312")
        except urllib.error.HTTPError as ex:
            self.mute.release()
            self.result = "openpage error: %s" % ex
            return False
        except ssl.SSLError as ex:
            self.mute.release()
            self.result = "openage  error: %s" % ex
            return False

        return self.result.find("京ICP证030173号")

    def getHtmlTdInfo(self, context):
        result = []
        p = re.compile(r"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}[\w|\W]+?\d{2,4}")
        ret = p.findall(str(context))

        if ret is None:
            return None

        for x in ret:
            element = []
            q = re.compile(r"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})[\w|\W]+?(\d{2,4})$")
            subRet = q.search(x);
            if subRet == None:
                continue
            element.append(subRet.group(1))
            element.append(subRet.group(2))
            result.append(element)
        return result

    def getAgencyIP(self, url=""):
        socket.setdefaulttimeout(20)  # 20秒内没有打开web页面，就算超时

        url = "http://www.xicidaili.com/nn/"  # 获取最新代理服务器
        self.result = str(urllib.request.urlopen(url).read())
        if self.result == None:
            return
        AgencyIP = self.getHtmlTdInfo(self.result)
        print(len(AgencyIP))

        # url = "http://www.xicidaili.com/nn/"  # 获取匿名访问代理服务器
        # self.result = str(urllib.request.urlopen(url).read())
        # if self.result == None:
        #     return
        # AgencyIP += self.getHtmlTdInfo(self.result)
        # print(len(AgencyIP))
        #
        # url = "http://5uproxy.net/http_non_anonymous.html"  # 获取透明访问代理服务器
        # self.result = str(urllib.request.urlopen(url).read())
        # if self.result == None:
        #     return
        # AgencyIP += self.getHtmlTdInfo(self.result)
        # print(len(AgencyIP))

        socket.setdefaulttimeout(5)  # 5内没有打开web页面，就算超时
        if len(AgencyIP) == 0:
            print("获取代理服务器失败")
            return
        for x in AgencyIP:
            if x[0] == "":
                continue
            try:
                proxy_support = urllib.request.ProxyHandler({'http': 'http://' + str(x[0]) + ':' + str(x[1])})
                self.opener = urllib.request.build_opener(proxy_support, urllib.request.HTTPHandler)
                urllib.request.install_opener(self.opener)
                if self.openpage() == False:
                    x[0] = ""
                    x[1] = ""
                    continue
                print("有效的代理服务器" + str(x[0]) + ":" + str(x[1]))
                print("等待2秒")
                time.sleep(2)
            except:
                continue

    def openpage_baidu(self, opener):  # url为相对路径
        try:
            url = "http://www.baidu.com"
            result = opener.open(url).read().decode("gb2312")
            return result.find("京ICP证030173号")
        except urllib.error.HTTPError as ex:
            self.mute.release()
            self.result = "openpage error: %s" % ex
            return False
        except ssl.SSLError as ex:
            self.mute.release()
            self.result = "openage  error: %s" % ex
            return False

    def request_for_baidu(self, ip, port):
        time.sleep(3)
        try:
            proxy = urllib.request.ProxyHandler({'http': 'http://' + ip + ':' + port})
            opener = urllib.request.build_opener(proxy, urllib.request.HTTPHandler)
            urllib.request.install_opener(opener)
            print(self.openpage_baidu(opener))
            return proxy
            # url = "http://www.baidu.com/"
            # proxies = {"http": "http://" + ip + ":" + port}
            # print(proxies)
            # r = requests.get(url=url, proxies=proxies)
            # if r.status_code == 200:
            #     print(r.content.encode('utf-8'))
            #     return proxies
        except BaseException as e:
            logging.error(e)

    def get_proxy_list(self, content):
        result = []
        p = re.compile(r'\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}[\w|\W]+?\d{2,4}[\w|\W]+?HTTP{,1}S{0,1}')
        ret = p.findall(str(content))

        if ret is None:
            print("not found")
            return None

        for x in ret:
            element = []
            q = re.compile(r"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})[\w|\W]+?(\d{2,4})[\w|\W]+?(HTTP{,1}S{0,1})$")
            subRet = q.search(x);
            if subRet == None:
                continue
            element.append(subRet.group(1))
            element.append(subRet.group(2))
            element.append(subRet.group(3))
            result.append(element)
            # print(element)
        return result

    def get_proxy_page(self, url=""):
        print(url)
        headers = {
            "User-Agent": "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"
        }

        response = requests.get(url, headers=headers)
        content = response.text.encode('utf-8')
        # print(content)

        list = self.get_proxy_list(content)
        return list


if __name__ == "__main__":
    test = OpenUrl()
    list = []
    for p in range(1, 2):
        list += test.get_proxy_page("http://www.xicidaili.com/wt/" + str(p))

    for item in list:
        result = test.request_for_baidu(item[0], item[1])
        print("Success:%s" % result)
        logging.info(result)
