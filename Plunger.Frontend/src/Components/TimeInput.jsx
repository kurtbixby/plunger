function TimeInput(props) {
    const {styles, name, value, onChange} = props;

    function fixupTimeString(timeString) {
        return timeString;
    }
    
    return <input
        className={styles}
        name={name}
        value={value}
        onChange={(e) => {
            e.target.value = fixupTimeString(e.target.value);
            onChange(e);
        }}
    />
}

export default TimeInput;