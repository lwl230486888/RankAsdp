import mysql.connector
import matplotlib.pyplot as plt

# MySQL数据库连接信息
host = 'localhost'
user = 'root'
password = 'your_password'
database = 'SDPdatabase'

# 连接到MySQL数据库
connection = mysql.connector.connect(
    host=host,
    user=user,
    database=database
)
query = 'SELECT Spare_Name, Spare_Actual_Quantity FROM spare_part ORDER BY Spare_Actual_Quantity DESC'
cursor = connection.cursor()
cursor.execute(query)
data = cursor.fetchall()

# 提取产品和库存数量，排除空值
labels = []
inventory = []
for row in data:
    if row[0] is not None and row[1] is not None:
        labels.append(row[0])
        inventory.append(row[1])

# 创建水平条形图
plt.barh(labels, inventory)

# 添加标题和标签
plt.title('Inventory Distribution')
plt.xlabel('Inventory Quantity')
plt.ylabel('Spare Part Name')

# 显示图表
plt.show()

# 关闭数据库连接
cursor.close()
connection.close()