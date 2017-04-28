package main

import (
	"fmt"
	"gopkg.in/mgo.v2"
	//"gopkg.in/mgo.v2/bson"
	//	"log"
)

type User struct {
	Id    int
	Name  string
	Phone string
}

func main() {

	session, err := mgo.Dial("localhost")
	if err != nil {
		panic(err)
	}
	defer session.Close()

	//collectionInfo := mgo.CollectionInfo{}
	users := session.DB("test").C("users")

	err = users.Insert(&User{Id: 1, Name: "Chetan Nadgouda", Phone: "425-283-8742"},
		&User{Id: 2, Name: "Aarati Khandekar", Phone: "425-879-6638"})
	if err != nil {
		fmt.Println("Error in inserting users", err)
		return
	}

    users.Find(

	fmt.Println("End of program")
}
