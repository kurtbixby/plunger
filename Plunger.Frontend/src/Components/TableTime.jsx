import TimeInput from "./TimeInput.jsx";

function TableTime(props) {
  const { styles, name, value, onChange, isEditable } = props;

  return !isEditable ? (
    <div>
      <p>{name}</p>
      <p>{value}</p>
    </div>
  ) : (
    <div>
      <label className="block" htmlFor={name.toLowerCase()}>
        {name}
      </label>
      <TimeInput name={name.toLowerCase()} value={value} onChange={onChange} />
    </div>
  );
}

export default TableTime;
