// This code is taken from vitess code base and is modified for our use.
package main

import (
	"time"

	"github.com/youtube/vitess/go/vt/logutil"
	logutilpb "github.com/youtube/vitess/go/vt/proto/logutil"
	vtctldatapb "github.com/youtube/vitess/go/vt/proto/vtctldata"
	vtctlservicepb "github.com/youtube/vitess/go/vt/proto/vtctlservice"
	"github.com/youtube/vitess/go/vt/vtctl/vtctlclient"
	"golang.org/x/net/context"
	"google.golang.org/grpc"
)

type gRPCVtctlClient struct {
	connection *grpc.ClientConn
	client     vtctlservicepb.VtctlClient
}

func gRPCVtctlClientFactory(addr string, dialTimeout time.Duration) (vtctlclient.VtctlClient, error) {
	// create the RPC client
	connection, err := grpc.Dial(addr, grpc.WithInsecure(), grpc.WithBlock(), grpc.WithTimeout(dialTimeout))
	if err != nil {
		return nil, err
	}
	client := vtctlservicepb.NewVtctlClient(connection)

	return &gRPCVtctlClient{
		connection: connection,
		client:     client,
	}, nil
}

// Private datatype that should not be exposed outside
type eventStreamAdapter struct {
	stream vtctlservicepb.Vtctl_ExecuteVtctlCommandClient
}

func (e *eventStreamAdapter) Recv() (*logutilpb.Event, error) {
	le, err := e.stream.Recv()
	if err != nil {
		return nil, err
	}
	return le.Event, nil
}

// ExecuteVtctlCommand is part of the VtctlClient interface
func (client *gRPCVtctlClient) ExecuteVtctlCommand(ctx context.Context, args []string, actionTimeout time.Duration) (logutil.EventStream, error) {
	query := &vtctldatapb.ExecuteVtctlCommandRequest{
		Args:          args,
		ActionTimeout: int64(actionTimeout.Nanoseconds()),
	}

	stream, err := client.client.ExecuteVtctlCommand(ctx, query)
	if err != nil {
		return nil, err
	}
	return &eventStreamAdapter{stream}, nil
}

// Close is part of the VtctlClient interface
func (client *gRPCVtctlClient) Close() {
	client.connection.Close()
}
