package main

import (
	"flag"
	"fmt"
	"io/ioutil"
	"time"

	//vtctlservicepb "github.com/youtube/vitess/go/vt/proto/vtctlservice"
	//"github.com/youtube/vitess/go/vt/vtctl/grpcvtctlclient"
	//"github.com/youtube/vitess/go/vt/vtctl/vtctlclient"
	"github.com/youtube/vitess/go/vt/wrangler"
	"golang.org/x/net/context"
	/*
		"github.com/youtube/vitess/go/exit"
		"github.com/youtube/vitess/go/vt/logutil"
		"github.com/youtube/vitess/go/vt/schemamanager"
		"github.com/youtube/vitess/go/vt/topo"
		"os"
		"os/signal"
		"syscall"
		"errors"
		"path"
		"math/rand"
		topodatapb "github.com/youtube/vitess/go/vt/proto/topodata"
		vschemapb "github.com/youtube/vitess/go/vt/proto/vschema"
		vttestpb "github.com/youtube/vitess/go/vt/proto/vttest"
		"github.com/youtube/vitess/go/vt/vtgate/vtgateconn"
		"github.com/youtube/vitess/go/vt/vtctl"
		"github.com/youtube/vitess/go/vt/vttablet/tmclient"
		"google.golang.org/grpc"
	*/)

var (
	updateStatementPtr = flag.Bool("update", false, "Set update to true to run DML. False => Query is run.")
	waitTime           = flag.Duration("wait-time", 24*time.Hour, "time to wait on an action")

	sql                     = flag.String("sql", "", "A list of semicolon-delimited SQL commands")
	allowLongUnavailability = flag.Bool("allow_long_unavailability", false, "Allow large schema changes which incur a longer unavailability of the database.")
	sqlFile                 = flag.String("sql-file", "", "Identifies the file that contains the SQL commands")
	waitSlaveTimeout        = flag.Duration("wait_slave_timeout", wrangler.DefaultWaitSlaveTimeout, "The amount of time to wait for slaves to receive the schema change via replication.")
	server                  = flag.String("server", "localhost:15999", "Vitess control server endpoint.")
	keyspace                = flag.String("keyspace", "", "Keyspace to run the schema script")
)

var (
	dialTimeout = 10 * time.Second
)

// getFileParam returns a string containing either flag is not "",
// or the content of the file named flagFile
func getFileParam(flag, flagFile, name string) (string, error) {
	if flag != "" {
		if flagFile != "" {
			return "", fmt.Errorf("action requires only one of %v or %v-file", name, name)
		}
		return flag, nil
	}

	if flagFile == "" {
		return "", fmt.Errorf("action requires one of %v or %v-file", name, name)
	}
	data, err := ioutil.ReadFile(flagFile)
	if err != nil {
		return "", fmt.Errorf("Cannot read file %v: %v", flagFile, err)
	}
	return string(data), nil
}

/*
// signal handling, centralized here
func installSignalHandlers(cancel func()) {
	sigChan := make(chan os.Signal, 1)
	signal.Notify(sigChan, syscall.SIGTERM, syscall.SIGINT)
	go func() {
		<-sigChan
		// we got a signal, cancel the current ctx
		cancel()
	}()
}
*/

func main() {

	fmt.Println("==========> Checkpoint 01")
	flag.Parse()

	client, err := gRPCVtctlClientFactory(*server, 10*time.Second)
	if err != nil {
		fmt.Printf("Failed to connect to localserver: %v\n", err)
		return
	}

	defer client.Close()
	if *keyspace == "" {
		fmt.Println("The <keyspace> argument is required for the commandApplySchema command")
		return
	}

	fmt.Println("==========> Checkpoint 02")
	fmt.Printf("Keyspace = %s.\n", *keyspace)
	change, err := getFileParam(*sql, *sqlFile, "sql")
	if err != nil {
		fmt.Printf("Could not find sql or sql-file parameter %v\n", err)
		return
	}
	fmt.Printf(" SQL ==> %s\n", change)

	ctx := context.Background()
	command := []string{"ApplySchema", "-sql", change, *keyspace}
	responseStream, err := client.ExecuteVtctlCommand(ctx, command, 10*time.Second)
	if err != nil {
		fmt.Printf("Error executing vtctl command. %v\n", err)
		return
	}

	got, err := responseStream.Recv()
	if err != nil {
		fmt.Printf("Error receiving response. %v\n", err)
		return
	}
	fmt.Printf("Response received ==> %s\n", got)
	fmt.Println("==========> Exit Checkpoint")
}
