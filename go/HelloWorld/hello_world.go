// Hello world!
package main

import "fmt"

type SomeStructure struct {
	someValue int
}

func main() {

	first := SomeStructure{someValue: 100}
	fmt.Printf("Some Value = %d\n", first.someValue)
}
