import WrappedSelect from "./WrappedSelect.jsx";

function TableSelect(props) {
  const { name, textValue, value, options, onSelect, isEditable } = props;
  return !isEditable ? (
    <div>
      <p>{name}</p>
      <p>{textValue}</p>
    </div>
  ) : (
    <div>
      <label className="block" htmlFor={name.toLowerCase()}>
        {name}
      </label>
      <WrappedSelect
        name={name.toLowerCase()}
        value={value}
        contents={options}
        onSelect={onSelect}
      />
    </div>
  );
}

export default TableSelect;
