import { useOnClickOutside } from "usehooks-ts";
import { useState, useRef } from "react";

function SelectDropDown(props) {
  const { contents, value, initialMessage, onSelect } = props;

  const [isOpen, setIsOpen] = useState(false);

  const ref = useRef(null);
  useOnClickOutside(ref, closeDropDown);

  function openDropDown() {
    setIsOpen(true);
  }
  function closeDropDown() {
    setIsOpen(false);
  }

  function toggleDropDown() {
    setIsOpen(!isOpen);
  }

  return (
    <>
      <button
        role="combobox"
        id="select"
        value="Select"
        aria-controls="listbox"
        aria-haspopup="listbox"
        tabIndex="0"
        aria-expanded="false"
        onClick={toggleDropDown}
      >
        {value ? value : initialMessage}
      </button>
      <ul className={!isOpen && "hidden"} role="listbox" id="listbox">
        {contents.map((item) => (
          <li
            key={item.id}
            role="option"
            onClick={() => {
              onSelect(item);
              closeDropDown();
            }}
          >
            {item.name}
          </li>
        ))}
      </ul>
    </>
  );
}

export default SelectDropDown;
