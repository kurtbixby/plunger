function SearchDropDownItem(props) {
  const { text, styles, callback } = props;
  return (
    <li className={styles ?? "hover:bg-amber-400"} onClick={callback}>
      {text}
    </li>
  );
}

export default SearchDropDownItem;
