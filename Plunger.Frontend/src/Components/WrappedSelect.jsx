function WrappedSelect(props) {
  const { name, value, contents, onSelect, styles } = props;
  return (
    <>
      <select
        className={styles ?? ""}
        name={name}
        value={value}
        onChange={onSelect}
      >
        {contents.map((p) => {
          return (
            <option key={p.id} value={p.id}>
              {p.name}
            </option>
          );
        })}
      </select>
    </>
  );
}

export default WrappedSelect;
