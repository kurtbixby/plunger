import Enums from "../Enums.js";

function TableCell(props) {
  const { title, value } = props;

  return (
    <div>
      <p>{title}</p>
      <p>{value}</p>
    </div>
  );
}

export default TableCell;
