#! /usr/bin/env python3
# -*-coding:utf-8-*-
import configparser
import logging

import os

import sys
from logging.handlers import TimedRotatingFileHandler

rootdir = os.path.abspath(sys.argv[0])
rootdir = os.path.dirname(rootdir) + "/"

cf = configparser.ConfigParser()
cf.read(rootdir + 'socket_server.conf')

formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s', datefmt='%Y/%m/%d %H:%M:%S')
logger = logging.getLogger("log")  # name
logger.setLevel(logging.DEBUG)
try:
    # 按1天进行日志切割，同时保存20天的日志
    fh = TimedRotatingFileHandler(rootdir + "log/" + "sock_server.log", "d", 1, 20)
except:
    os.makedirs(rootdir + "log/")
    fh = TimedRotatingFileHandler(rootdir + "log/" + "sock_server.log", "d", 1, 20)
fh.setLevel(logging.DEBUG)
fh.setFormatter(formatter)
# 给log 添加Handler
logger.addHandler(fh)
