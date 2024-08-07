function WrappedSelect(props) {
    const { name, value, contents, onSelect } = props;
    return <>
        <select name={name} value={value} onChange={e => onSelect(e.target.value)}>
            {contents.map(p => {
                console.log(p);
                return <option key={p.id} value={p.id}>{p.name}</option>
            })}
        </select>
    </>
}

export default WrappedSelect;