import { useQuery } from "@tanstack/react-query";
import fetchHomePageLists from "../Hooks/fetchHomePageLists";
import SmallList from "../Components/SmallList";
import AddGameWidget from "../Widgets/AddGameWidget";
import NowPlayingWidget from "../Widgets/NowPlayingWidget";
import {useCurrentUser} from "../CurrentUserProvider.jsx";
import {queryKeyConstants} from "../Hooks/queryKeyConstants.js";
import ListWidget from "../Widgets/ListWidget.jsx";

function UserHomePage() {
    const { state: { isLoading, isLoggedIn, user: currentUser } } = useCurrentUser();

  const results = useQuery({
    queryKey: [queryKeyConstants.homePageLists, currentUser === null ? 0 : currentUser.userId],
    queryFn: fetchHomePageLists,
  });
  const lists = isLoggedIn ? (results?.data ?? []) : [];

  return (
    <div className="container lg:mx-auto">
        {isLoggedIn && <>
          <div className="grid grid-cols-2 gap-3">
            <AddGameWidget />
            {console.log(lists)}
            {!lists.length ? (
              <h1>No Lists</h1>
            ) : (
              lists.map((list) => list.type === 1 ? <NowPlayingWidget key={list.id} list={list}/> : <ListWidget key={list.id} list={list} />)
            )}
          </div>
        </>}
    </div>
  );
}

export default UserHomePage;
