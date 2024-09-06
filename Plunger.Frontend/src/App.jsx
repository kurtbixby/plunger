import {useEffect, useState} from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import CurrentUserContext from "./CurrentUserContext";
import TopNavigation from "./Widgets/TopNavigation";
import HomePage from "./Pages/HomePage";
import ProfilePage from "./Pages/ProfilePage";
import CollectionPage from "./Pages/CollectionPage";
import ListsPage from "./Pages/ListsPage";
import TokenManagement from "./TokenManagement";
import APICalls from "./APICalls.js";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

function App() {
  const [userState, setUserState] = useState({loggedIn: false, userName: null, userId: null});

  useEffect(() => {
    (async () => await initializeUser())();
  }, []);
  
  async function initializeUser() {
    const storedToken = TokenManagement.loadToken();
    if (storedToken === null) {
      return;
    }
    
    const userDetails = await APICalls.sendTokenLoginRequest(storedToken);
    if (!userDetails) {
      return;
    }
    
    setUserState({loggedIn: true, userName: userDetails.userName, userId: userDetails.userId});
  }
  
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <CurrentUserContext.Provider value={[userState, setUserState]}>
          <TopNavigation />
          
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/:userName" element={<ProfilePage />} />
            <Route path="/:userName/collection" element={<CollectionPage />} />
            {/*<Route path="/:userName/lists" element={<ListsPage />} />*/}
          </Routes>
        </CurrentUserContext.Provider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
