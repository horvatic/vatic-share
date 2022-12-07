module github.com/horvatic/vatic-share/boot-strap

go 1.19

replace github.com/horvatic/vatic-share/sharedConstants => ../shared-constants

require (
	github.com/horvatic/vatic-share/sharedConstants v0.0.0-00010101000000-000000000000
	golang.org/x/sys v0.2.0
)
