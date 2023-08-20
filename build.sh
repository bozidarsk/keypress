# build: mono
# runtime: mono, evemu

mcs '-recurse:*.cs' -define:NO_FILES,NO_CSS -out:keypress
if [[ $? -eq 0 ]]; then
	chmod 755 ./keypress
	sudo chown root:root ./keypress
	sudo mv ./keypress /usr/local/bin
else
	echo "Build failed."
	exit 1
fi
