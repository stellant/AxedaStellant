#!/bin/sh

CHAT=/usr/sbin/chat
ROUTE=/sbin/route

# save and remove default route when started as root
uid=`id -u`
default_route=`$ROUTE -n | sed -n '/^0\.0\.0\.0/s/^[\.0-9]\+[\ ]\+\([\.0-9]\+\).[\ ]\+.\+/\1/p'`
if [ "$uid" = 0 -a -n "$default_route" ]; then
	echo "$default_route" > default-route
	$ROUTE del default
fi

# fix init string
if [ -z "$EDIALER_INIT_STRING" ]; then
	INIT_STRING=AT
else
	INIT_STRING=$EDIALER_INIT_STRING
fi

# fix dial prefix
if [ -z "$EDIALER_DIAL_PREFIX" ]; then
	DIAL_PREFIX=ATDT
else
	DIAL_PREFIX=$EDIALER_DIAL_PREFIX
fi

# initialize modem, dial phone number and wait for PPP handshake
$CHAT -v                                                        \
        TIMEOUT         3                                       \
        ABORT           'NO CARRIER\r'                          \
        ABORT           'NO DIALTONE\r'                         \
        ABORT           'NO DIAL TONE\r'                        \
        ABORT           'BUSY\r'                                \
        ABORT           'VOICE\r'                               \
        ABORT           'NO ANSWER\r'                           \
        ''              ${INIT_STRING}                          \
        OK              ${DIAL_PREFIX}${EDIALER_PHONE_NUMBER}   \
        TIMEOUT         60                                      \
        CONNECT         '\c'                                    \
        TIMEOUT         5                                       \
        '!}!}'          ''

rc=$?

# try to authenticate interactively if timeout occured
if [ $rc = 3 ]; then
        $CHAT -v                                                \
            TIMEOUT         5                                   \
            ''              '\r'				\
	    ogin:--name:    ${EDIALER_USER_NAME}                \
            assword:        '\q'${EDIALER_PASSWORD}
        rc=$?
fi

# restore default route in case of failure
if [ $rc != 0 -a "$uid" = 0 -a -n "$default_route" ]; then
        $ROUTE add default gw "$default_route"
        rm -f default-route
fi

exit "$rc"

