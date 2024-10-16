import { dateFormat } from "../Utils.js";

function TableDate(props) {
  const { styles, name, value, onChange, isEditable } = props;

  function fixupDate(date) {
    let dateObj = new Date(date);

    const components = [
      dateObj.getUTCFullYear(),
      String(dateObj.getUTCMonth() + 1).padStart(2, "0"),
      String(dateObj.getUTCDate()).padStart(2, "0"),
    ];

    const str = components.join("-");

    // const str = dateObj.toLocaleDateString(undefined, options);
    return str;
  }

  return !isEditable ? (
    <div>
      <p>{name}</p>
      {value ? <p>{dateFormat(value)}</p> : <p>N/A</p>}
    </div>
  ) : (
    <div>
      <label className="block" htmlFor={name.toLowerCase()}>
        {name}
      </label>
      <input
        name={name.toLowerCase()}
        type="date"
        value={fixupDate(value)}
        onChange={onChange}
      />
    </div>
  );
}

export default TableDate;
