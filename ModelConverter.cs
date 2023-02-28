using System.Diagnostics;
using System.Reflection;

namespace DbSetToModelConverter
{
    public class ModelConverter
    {
        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
        new Dictionary<Type, ICollection<PropertyInfo>>();
        public IQueryable<T> ObjectListToModelList<T>(IEnumerable<object> convertList) where T : class, new()
        {
            List<T> newModel = new List<T>();
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }


                foreach (var row in convertList.AsEnumerable())
                {
                    T obj = new T();


                    foreach (var prop in properties)
                    {

                        try
                        {
                            PropertyInfo[] array = row.GetType().GetProperties();
                            for (int i = 0; i < array.Length; i++)
                            {
                                PropertyInfo rowProps = array[i];

                                if (rowProps.Name == prop.Name)
                                {
                                    var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                    var safeValue = row.GetType().GetProperty(rowProps.Name).GetValue(row) == null ? null : Convert.ChangeType(row.GetType().GetProperty(rowProps.Name).GetValue(row), propType);
                                    prop.SetValue(obj, safeValue, null);
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            continue;

                        }
                    }
                    newModel.Add((T)obj);


                }

                return newModel.AsQueryable();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return newModel.AsQueryable();

            }

        }

        public  IQueryable<T> ModelListToObjectList<T>(IEnumerable<object> convertList) where T : class, new()
        {
            List<T> newModel = new List<T>();
            try
            {
                var objType = convertList.FirstOrDefault().GetType();
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }


                foreach (var row in convertList.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            for (int i = 0; i < properties.ToList().Count; i++)
                            {
                                PropertyInfo rowProps = properties.ToList()[i];
                                if (rowProps.Name == prop.Name)
                                {
                                    var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                    var safeValue = row.GetType().GetProperty(rowProps.Name).GetValue(row) == null ? null : Convert.ChangeType(row.GetType().GetProperty(rowProps.Name).GetValue(row), propType);
                                    prop.SetValue(obj, safeValue, null);
                                    break;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            continue;

                        }
                    }
                    newModel.Add((T)obj);


                }

                return newModel.AsQueryable();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return newModel.AsQueryable();

            }

        }

        public T ObjectToModel<T>(object convertObject) where T : class, new()
        {
            T obj = new T();
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

                foreach (var prop in properties)
                {

                    try
                    {
                        foreach (var rowProps in convertObject.GetType().GetProperties())
                        {
                            if (rowProps.Name == prop.Name)
                            {
                                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                var safeValue = convertObject.GetType().GetProperty(rowProps.Name).GetValue(convertObject) == null ? null : Convert.ChangeType(convertObject.GetType().GetProperty(rowProps.Name).GetValue(convertObject), propType);
                                prop.SetValue(obj, safeValue, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        continue;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return obj;

            }
        }

        public T ModelToObject<T>(object donusecekObje) where T : class, new()
        {
            T obj = new T();
            try
            {
                var objType = donusecekObje.GetType();
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

                foreach (var prop in obj.GetType().GetProperties())
                {

                    try
                    {
                        foreach (var rowProps in properties)
                        {
                            if (rowProps.Name == prop.Name)
                            {
                                var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                var safeValue = donusecekObje.GetType().GetProperty(rowProps.Name).GetValue(donusecekObje) == null ? null : Convert.ChangeType(donusecekObje.GetType().GetProperty(rowProps.Name).GetValue(donusecekObje), propType);
                                prop.SetValue(obj, safeValue, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        continue;
                    }
                }
                return obj;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return obj;

            }
        }

    }
}