import { useEffect, useState } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import TopNavigation from "./Widgets/TopNavigation";
import UserHomePage from "./Pages/UserHomePage.jsx";
import ProfilePage from "./Pages/ProfilePage";
import CollectionPage from "./Pages/CollectionPage";
import ListsPage from "./Pages/ListsPage";
import TokenManagement from "./TokenManagement";
import APICalls from "./APICalls.js";
import { CurrentUserProvider } from "./CurrentUserProvider.jsx";
import StatusListPage from "./Pages/StatusListPage.jsx";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

function App() {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <CurrentUserProvider>
          <TopNavigation />
          <Routes>
            <Route path="/" element={<UserHomePage />} />
            <Route path="/:userName" element={<ProfilePage />} />
            <Route path="/:userName/collection" element={<CollectionPage />} />
            <Route path="/:userName/gamestates" element={<StatusListPage />} />
            {/*<Route path="/:userName/lists" element={<ListsPage />} />*/}
          </Routes>
        </CurrentUserProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
