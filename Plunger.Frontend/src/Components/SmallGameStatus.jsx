import { useState } from "react";
import WrappedSelect from "./WrappedSelect.jsx";
import UIConstants from "../UIConstants.js";
import { dateFormat } from "../Utils.js";
import modifyGameStatus from "../Hooks/modifyGameStatus.js";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {queryKeyConstants} from "../Hooks/queryKeyConstants.js";
import SmallRowElement from "./SmallRowElement.jsx";
import GameStatusWidget from "../Widgets/GameStatusWidget.jsx";

function SmallGameStatus(props) {
    const { game: { game, status} } = props;
    
  return (
    <div className="grid grid-cols-2">
      <img src={game.coverUrl} alt="" />
        <GameStatusWidget game={{game, status}}/>
    </div>
  );
}

export default SmallGameStatus;
