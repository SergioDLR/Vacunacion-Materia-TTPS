import AdminNavBar from "./AdminNavBar";
const AdminWraper = ({ children }) => {
  return (
    <>
      <AdminNavBar />
      {children}
    </>
  );
};

export default AdminWraper;
