import os
import sys

def count_csharp_lines(directory):
    """
    统计指定目录下所有C#文件(.cs)的总行数。
    
    :param directory: 要统计的目录路径
    :return: 总行数
    """
    total_lines = 0
    # 尝试的编码列表，按常见程度排序
    encodings = ['utf-8', 'utf-16', 'gbk', 'iso-8859-1']
    
    for root, dirs, files in os.walk(directory):
        for file in files:
            if not file.endswith('.cs'):
                continue  # 跳过非C#文件
            
            file_path = os.path.join(root, file)
            line_count = 0
            success = False  # 标记是否成功读取文件
            
            # 尝试不同编码读取文件
            for encoding in encodings:
                try:
                    with open(file_path, 'r', encoding=encoding) as f:
                        current = 0
                        for line in f:
                            current += 1
                        line_count = current
                        success = True
                        break  # 成功读取，跳出编码尝试循环
                except UnicodeDecodeError:
                    continue  # 编码错误，尝试下一个
                except PermissionError:
                    print(f"错误：没有权限读取文件 '{file_path}'，跳过。")
                    break
                except Exception as e:
                    print(f"读取文件 '{file_path}' 时发生错误：{str(e)}，跳过。")
                    break
            
            if success:
                total_lines += line_count
            else:
                print(f"警告：无法解码文件 '{file_path}'，已跳过。")
    
    return total_lines

if __name__ == "__main__":
    # 获取用户输入的路径
    if len(sys.argv) > 1:
        path = sys.argv[1]
    else:
        path = "."
    
    # 验证路径是否存在
    if not os.path.isdir(path):
        print(f"错误：路径 '{path}' 不存在或不是目录。")
        sys.exit(1)
    
    # 统计并输出结果
    total = count_csharp_lines(path)
    print(f"总计：所有C#文件共 {total} 行代码。")