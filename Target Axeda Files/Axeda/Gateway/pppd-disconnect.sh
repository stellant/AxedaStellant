#!/bin/sh

ROUTE=/sbin/route

uid=`id -u`
default_route=`cat default-route 2>/dev/null`
if [ "$uid" = 0 -a -n "$default_route" ]; then
	$ROUTE add default gw "$default_route"
	rm -f default-route
fi
