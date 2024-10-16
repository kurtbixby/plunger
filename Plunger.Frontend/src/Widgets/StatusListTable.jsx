import StatusTableRow from "../Components/StatusTableRow.jsx";

function StatusListTable(props) {
  const { gameStatuses } = props;

  return (
    <div className={"basis-11/12"}>
      <div className={"flex justify-center text-center"}>
        <p className={"basis-4/12"}>Game</p>
        <p className={"basis-1/12"}>Play State</p>
        <p className={"basis-1/12"}>Time Played</p>
        <p className={"basis-1/12"}>Date Started</p>
        <p className={"basis-1/12"}>Completed</p>
        <p className={"basis-1/12"}>Date Completed</p>
      </div>
      {gameStatuses?.map((gs) => (
        <StatusTableRow key={gs.id} gameStatus={gs} />
      ))}
    </div>
  );
}

export default StatusListTable;
