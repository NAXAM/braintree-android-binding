# PayPal DataCollector
sh ./paypal-datacollector/build.sh
cp -p ./paypal-datacollector/*.nupkg ./nugets/

# PayPal DataCollector
sh ./braintree-core/build.sh
cp -p ./braintree-core/*.nupkg ./nugets/

# PayPal DataCollector
sh ./paypal-onetouch/build.sh
cp -p ./paypal-onetouch/*.nupkg ./nugets/

sh ./build.sh
cp -p ./*.nupkg ./nugets/